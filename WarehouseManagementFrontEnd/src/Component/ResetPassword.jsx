import {useParams, Navigate, useNavigate} from "react-router-dom"
import {useState, useEffect} from "react"
import {useForm} from "react-hook-form";

export default function ResetPassword(props) {
    const {register, handleSubmit} = useForm();
    const [error, setError] = useState("")
    const [loading, setLoading] = useState(false)
    const navigate = useNavigate();
    const [ShowPass, setShowPass] = useState(false)
    const [success, setSuccess] = useState("")
    const {id, code} = useParams()
    const [valid, setValid] = useState(true)
    const onSubmit = data => sendChange(data);

    function showpassonchange(e) {
        setShowPass(e.target.checked)
    }
    const checkId=async ()=>{
        const response = await fetch(`https://localhost:7075/api/Account/checkForReset/${id}`, {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
        })
        if (!response.ok) {
          return false
        }
    }
     const checkvalid=(data)=> {
        setError("");
        if (data.password.length < 4) {
            setError("Độ dài mật khẩu phải phải lớn hơn 4!")
            return false
        }
         if (data.password.length ===0||data.confirmPassword.length===0) {
             setError("Bạn chưa nhập thông tin đầy đủ!")
             return false
         }
        if (data.password !== data.confirmPassword) {
            setError("Mật khẩu không giống với mật khẩu xác nhận!")
            return false
        }
        return true
    }

    async function sendChange(data) {
        if (checkvalid(data)) {
            setLoading(true);
            const response = await fetch(`https://localhost:7075/api/Account/ResetPassword/${id}/${code}`, {
                method: 'PUT',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify(data.password)
            })
            if (!response.ok) {
                const content = await response.json()
                setError("Lỗi đã xảy ra!")
            } else {
                const content = await response.text()
                setSuccess(content)
            }
            setLoading(false)
        }
    }

    useEffect(() => {
          if(!checkId()){
              navigate("/error")
          }
    }, [])
    if (props.isLogged) {
        return <Navigate to="/Error"></Navigate>
    }
    return (
        <>
            <div className="container-fluid h-100 d-flex flex-column">
                <div className="container w-50 m-auto d-flex flex-column justify-content-center p-5">
                    <h1 className="text-center">Đặt lại mật khẩu</h1>
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <div className="row mb-1">
                            <div className="col-6">
                                <div className="form-floating">
                                    <input type={ShowPass ? "text" : "password"}
                                           className="form-control rounded-3 border-2"
                                           id="newPass" placeholder="pass" {...register("password")}/>
                                    <label htmlFor="newPass">Mật khẩu</label>
                                </div>
                            </div>
                            <div className="col-6">
                                <div className="form-floating">
                                    <input type={ShowPass ? "text" : "password"}
                                           className="form-control rounded-3 border-2"
                                           id="conPass" placeholder="conpass" {...register("confirmPassword")}
                                          />
                                    <label htmlFor="conPass">Xác nhận mật khẩu</label>
                                </div>
                            </div>
                        </div>
                        <h5 className="text-danger text-center fw-bold">
                            {error}
                        </h5>
                        <div className="form-check mb-1">
                            <input className="form-check-input rounded-0" type="checkbox" value=""
                                   onChange={(e) => showpassonchange(e)} id="ShowPassword"/>
                            <label className="form-check-label" htmlFor="ShowPassword">Hiện mật khẩu</label>
                        </div>

                        {success === "" ?
                            <button className={`btn btn-outline-success rounded-5 fw-bolder border-3 w-100  ${loading ? "disabled" : ""}`} type="submit">{loading ?
                                <>
                                    <span className="spinner-grow spinner-grow-sm ms-1" role="status" aria-hidden="true"></span>
                                    <span className="spinner-grow spinner-grow-sm ms-1" role="status" aria-hidden="true"></span>
                                    <span className="spinner-grow spinner-grow-sm ms-1" role="status" aria-hidden="true"></span>
                                </> : <>
                                    Đặt lai mật khẩu
                                </>
                            }</button>
                            :
                            <button className="btn btn-dark w-100 rounded-5 fw-bolder border-3"
                                    onClick={() => props.navigate("/")}>Quay về</button>
                        }

                    </form>
                </div>
            </div>
        </>
    )
}