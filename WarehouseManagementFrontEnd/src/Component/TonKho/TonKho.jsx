import {useEffect, useState} from "react";
import {useTheme} from "@table-library/react-table-library/theme";
import {CompactTable} from "@table-library/react-table-library/compact";
import * as XLSX from "xlsx";
import {Navigate} from "react-router-dom";
import {SortToggleType, useSort} from "@table-library/react-table-library/sort";
import {usePagination} from "@table-library/react-table-library/pagination";

export default function TonKho(props) {
    const [loading, setLoading] = useState(false);
    const [nodes, setNodes] = useState([]);
    const [productList, setProductList] = useState([]);
    const [showProductList, setShowProductList] = useState([]);
    const [deleteModal, setDeleteModal] = useState(false)
    const [deleteModel, setDeleteModel] = useState({
        id: "",
        name: ""
    });
    const [addMode, setAddMode] = useState(false);
    const [addModel, setAddModel] = useState({
        productId: "",
        quantity: 1,
    });
    const [editMode, setEditMode] = useState(false);
    const [editTurn, setEditTurn] = useState("");
    const [editOldValue, setEditOldValue] = useState(0);

    const [err, setError] = useState("");
    const getProductList = async () => {
        const response = await fetch('https://localhost:7075/api/v1/Products', {
            headers: {'Content-Type': 'application/json'},
            credentials: 'include',
            method: "GET"
        });
        if (!response.ok) {
            const text = await response.text();
            throw Error(text);
        }
        const content = await response.json();
        setProductList(content);
    }
    const getList = async () => {
        setLoading(true)
        const response = await fetch('https://localhost:7075/api/v1/stocks', {
            headers: {'Content-Type': 'application/json'},
            credentials: 'include',
            method: "GET"
        });
        if (!response.ok) {
            const text = await response.text();
            setLoading(false)
            throw Error(text);
        }
        const content = await response.json();
        setNodes(content)
        setLoading(false)
    }
    const downloadExcel = (data) => {
        if(nodes.length>0){
        let list = []
        nodes.map(n =>
            list.push({
                Id: n.id,
                name: n.name,
                quantity: n.quantity,
            })
        )
        const worksheet = XLSX.utils.json_to_sheet(list);
        worksheet.A1.v = "Id";
        worksheet.B1.v = "Tên";
        worksheet.C1.v = "Số lượng";
        const workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, "Tồn kho");
        XLSX.writeFile(workbook, "TonKho.xlsx");
        }
    };
    const Add = async () => {
        const response = await fetch('https://localhost:7075/api/v1/stocks', {
            headers: {'Content-Type': 'application/json'},
            credentials: 'include',
            method: "Post",
            body: JSON.stringify(addModel)
        });
        if (!response.ok) {
            const text = await response.text();
            return false
        }
        await getList()
    }
    const Delete = async (id) => {
        const response = await fetch(`https://localhost:7075/api/v1/stocks/${id}`, {
            headers: {'Content-Type': 'application/json'},
            credentials: 'include',
            method: "Delete",
        });
        if (!response.ok) {
            const text = await response.text();
            return false
        }
        setDeleteModal(false)
        setDeleteModel({
            id: "",
            name: ""
        })
        await getList()
        return true
    }
    const Update = async (id, quantity) => {
        const response = await fetch('https://localhost:7075/api/v1/stocks', {
            headers: {'Content-Type': 'application/json'},
            credentials: 'include',
            method: "Put",
            body: JSON.stringify({
                productId: id,
                quantity: quantity
            })
        });
        if (!response.ok) {
            const text = await response.text();
            return false
        }
        return true
    }

    //Table
    const data = {nodes};
    const sort = useSort(
        data,
        {
            onChange: onSortChange,
        },
        {
            sortToggleType: SortToggleType.AlternateWithReset,
            sortFns: {
                id: (array) => array.sort((a, b) => a.id.localeCompare(b.id)),
                ten: (array) => array.sort((a, b) => a.name.localeCompare(b.name.name)),
                soluong: (array) => array.sort((a, b) => a.quantity - b.quantity),
            },
        }
    );
    const pagination = usePagination(data, {
        state: {
            page: 0,
            size: 10,
        },
        onChange: onPaginationChange,
    });

    function onPaginationChange(action, state) {

    }

    function onSortChange(action, state) {
    }

    const theme = useTheme({
        HeaderRow: `
        .th {
          border: 1px solid black;
          border-bottom: 3px solid black;
           background-color: #eaf5fd;
        }
      `,
        BaseCell: `
        
      `,
        Row: `
        cursor: pointer;
        .td {
          border: 1px solid black;
          
          background-color: #9eb0f7;
        }

        &:hover .td {
          border-top: 1px solid orange;
          border-bottom: 1px solid orange;
        }
      `,
        Table: `
        --data-table-library_grid-template-columns:  1fr 1fr 1fr 1fr;
      `,
    });
    const COLUMNS = [
        {label: 'Id', renderCell: (item) => item.id, sort: {sortKey: "id"}},
        {label: 'Tên sản phẩm', renderCell: (item) => item.name, sort: {sortKey: "ten"}},
        {
            label: 'Số lượng',
            renderCell: (item) => editMode && editTurn === item.id ?
                <input className="form-control rounded-0 w-100" onKeyDown={(e)=>handleKeyDown1(e,item.id,item.quantity)} onChange={(e) => updateQuantity(item.id, e)} type="number"
                       value={item.quantity}/> : item.quantity, sort: {sortKey: "soluong"}
        },
        {
            label: '', renderCell: (item) =>
                editMode ?
                    editTurn === item.id ?
                        <div className="d-flex gap-5 justify-content-center align-items-center">
                            <button className="btn btn-secondary" onClick={() => {
                                setEditTurn("")
                                setEditMode(false)
                                CancelEdit(item.id, editOldValue)
                                setEditOldValue(0)
                            }}>Hủy
                            </button>
                            <button className="btn btn-primary" onClick={() => {
                                if (!Update(item.id, item.quantity)) {
                                    setError("Lỗi")
                                } else {
                                    setEditTurn("")
                                    setEditMode(false)
                                    setEditOldValue(0)
                                }
                            }}>Sửa
                            </button>
                        </div> : ""
                    :
                    <div className="d-flex gap-5 justify-content-center align-items-center">
                        <button className="btn btn-primary" onClick={() => {
                            setEditTurn(item.id)
                            setEditMode(true)
                            setEditOldValue(item.quantity)
                        }}>Thay đổi
                        </button>
                        <button className="btn btn-danger" onClick={() => {
                            setDeleteModal(true)
                            setDeleteModel({
                                id: item.id,
                                name: item.name
                            })
                        }}>Xóa
                        </button>
                    </div>
        },
    ];
    const updateShowProductList = (l) => {
        let list = productList.filter(p =>
            !l.some(n => n.id === p.id)
        )
        setShowProductList(list)
    }
    const updateQuantity = (id, e) => {
        let list = []
        nodes.map(s => {
            if (s.id === id) {
                let stock = {...s};
                if (e.target.value >= 0)
                    stock.quantity = e.target.value;
                else
                    stock.quantity = 0
                list.push(stock);
            } else
                list.push(s);
        });
        setNodes(list)
    }
    const CancelEdit = (id, old) => {
        let list = []
        nodes.map(s => {
            if (s.id === id) {
                let stock = {...s};
                stock.quantity = old;
                list.push(stock);
            } else
                list.push(s);
        });
        setNodes(list)
    }
    const changeProductId = (e) => {
        setAddModel({
            productId: e.target.value,
            quantity: addModel.quantity
        })
    }
    const changeQuantity = (e) => {
        setAddModel({
            product: addModel.product,
            quantity: e.target.value
        })
    }
    function handleKeyDown(event) {
        if (event.key === 'Enter' && addMode) {
            Add();
        }
    }
    function handleKeyDown1(event,id,quantity) {
        if (event.key === 'Enter' && editMode) {
            if (!Update(id, quantity)) {
                setError("Lỗi")
            } else {
                setEditTurn("")
                setEditMode(false)
                setEditOldValue(0)
            }

        }
    }
    //UseEffect
    useEffect(() => {
        getList()
        getProductList()
    }, []);
    useEffect(() => {
        updateShowProductList(nodes)
    }, [nodes.length]);
    useEffect(() => {
        updateShowProductList(nodes)
    }, [addMode]);
    if (!props.user.isLogged && props.user.userId === "") {
        return <Navigate to="/login"></Navigate>
    }
    return (<>
        <div className="p-5 pt-0">
            <h1 className="pt-4 text-center">Danh sách tồn kho</h1>
            {loading ?
                <div className="m-auto">
                    <div className="d-flex flex-row gap-3 justify-content-center">
                        <div className="spinner-grow" role="status">
                        </div>
                        <div className="spinner-grow" role="status">
                        </div>
                        <div className="spinner-grow" role="status">
                        </div>
                    </div>
                    <p className="text-center">Loading</p>
                </div>
                :
                <div>
                    {!addMode ?
                        <div className="d-flex gap-3">
                            <button className="btn btn-outline-primary fw-bold border-3 mb-3"
                                    onClick={() => setAddMode(true)}><i className="bi bi-plus-circle"> Thêm
                                sản phẩm vào
                                kho</i>
                            </button>
                            <button className={`btn btn-outline-success fw-bold border-3 mb-3 ${nodes.length<=0? "disabled":""} `}
                                    onClick={() => downloadExcel(nodes)}>Tạo ra file Excel
                            </button>
                        </div>
                        :
                        <div className="d-flex flex-column">
                            <div>
                                {!addMode ?
                                    <div>
                                        <button className="btn btn-success" onClick={() => setAddMode(true)}>Thêm sản
                                            phẩm vào
                                            kho
                                        </button>
                                    </div>
                                    :
                                    <div>
                                        <button className="btn btn-outline-secondary fw-bold border-3 w-25"
                                                onClick={() => setAddMode(false)}>Hủy
                                        </button>
                                        <div className="d-flex pt-3 pb-3 gap-5">
                                            <div className="form-floating w-100">
                                                <select className="form-select" id="floatingSelect"
                                                        onChange={changeProductId}
                                                        aria-label="Floating label select example"
                                                        value={addModel.product}>
                                                    <option value="" selected disabled>Lựa chọn sản phẩm</option>
                                                    {showProductList.map(product =>
                                                        <option key={product.id}
                                                                value={product.id}>{product.name} - {product.id}</option>
                                                    )}
                                                </select>
                                                <label htmlFor="floatingSelect">Sản phẩm</label>
                                            </div>
                                            <div className="d-flex fl">
                                                <div className="form-floating ">
                                                    <input onKeyDown={handleKeyDown} type={"number"} className="form-control rounded-0 border-2"
                                                           value={addModel.quantity} onChange={changeQuantity}
                                                           id="floatingInput"
                                                           placeholder="Quantity"/>
                                                    <label htmlFor="floatingInput">Số lượng</label>
                                                </div>
                                            </div>
                                            <div className="d-flex justify-content-center w-25">
                                                <button className="btn btn-success m-auto w-100"
                                                        onClick={() => Add()}>Thêm
                                                </button>
                                            </div>
                                        </div>
                                    </div>}
                            </div>
                        </div>
                    }
                    <CompactTable columns={COLUMNS} data={data} theme={theme} sort={sort} pagination={pagination}
                                  layout={{custom: true, horizontalScroll: true}}/>
                    {nodes.length === 0 ? <p className="text-center">Không có dữ liệu </p> :
                        <div className="d-flex justify-content-end">
                       <span>
          Trang:{" "}
                           {pagination.state.getPages(data.nodes).map((_, index) => (
                               <button
                                   className={`btn ${pagination.state.page === index ? "btn-dark" : "btn-outline-dark"} btn-sm`}
                                   key={index}
                                   type="button"
                                   style={{
                                       marginRight: "5px",
                                       fontWeight: pagination.state.page === index ? "bold" : "normal",
                                   }}
                                   onClick={() => pagination.fns.onSetPage(index)}
                               >
                                   {index + 1}
                               </button>
                           ))}
        </span>
                        </div>}
                    <div className={'modalpanel ' + (deleteModal ? "modal-active" : "")}>
                        <div
                            className='modalpanel-content rounded-0  bg-white m-auto d-flex justify-content-between flex-column'>
                            <div className='container-fluid d-flex justify-content-center'>
                                <p className="h1">Xóa sản phẩm trong kho: {deleteModel.name}</p>
                            </div>
                            <div className='modalpanel-content-text p-3'>
                                Bạn có muốn xóa sản phẩm có id là {deleteModel.id} ?
                            </div>
                            <div className='align-bottom d-flex gap-3 justify-content-center p-2'>
                                <button className='btn btn-secondary w-50'
                                        onClick={() => setDeleteModal(false)}>Hủy
                                </button>
                                <button className='btn btn-danger w-50' onClick={() => Delete(deleteModel.id)}>Ok
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            }

        </div>
    </>)
}