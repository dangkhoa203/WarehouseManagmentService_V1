/* eslint-disable react/prop-types */
import {useEffect, useState} from "react";
import { useForm } from "react-hook-form";
import {Navigate, Link, useNavigate} from "react-router-dom";
export default function Login(props) {
    const navigate=useNavigate();
    const [error, setError] = useState("")
    const [loading, setLoading] = useState(false)
    const { register, handleSubmit, watch, formState: { errors } } = useForm();
    const onSubmit = data => login(data);
    const [ShowPass, setShowPass] = useState(false)
    const checkData=(data)=>{
        setError("")
        if(data.username.length===0||data.password.length===0){
            setError("Bạn chưa nhập thông tin đầy đủ!")
            return false;
        }
        return true;
    }
    async function login(data) {
        if(checkData(data)) {
            try {
                setLoading(true)
                if (data.username === "" || data.password === "") {
                    setError("Please enter you credential")
                    throw Error("Info error");
                }
                const response = await fetch('https://localhost:7075/api/Account/Login', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    credentials: 'include',
                    body: JSON.stringify(data)
                })
                if (!response.ok) {
                    const text = await response.text();
                    setError(text)
                    setLoading(false)
                    throw Error(text);
                }
                await props.getInfo()
                setLoading(false)
            } catch (e) {
                setLoading(false)
            }
        }
    }

    function showpassonchange(e) {
        setShowPass(e.target.checked)
    }
    if (props.user.isLogged) {
        return <Navigate to="/"></Navigate>
    }
    return (
        <>
                <div className="container d-flex justify-content-center p-0 h-100">
                    <form className="w-50" onSubmit={handleSubmit(onSubmit)}>
                        <p className="h1 text-center mb-3">Đăng nhập</p>
                        <div className="d-flex flex-column">
                            <div className="form-floating mb-3">
                                <input type="text" className="form-control rounded-4 border-2"
                                       id="floatingInput" {...register("username")} placeholder="username"/>
                                <label htmlFor="floatingInput">Tên đăng nhập</label>
                            </div>
                            <div className="form-floating">
                                <input type={ShowPass ? "text" : "password"} className="form-control rounded-4 border-2"
                                       id="floatingInput" {...register("password")} placeholder="password"/>
                                <label htmlFor="floatingInput">Mật khẩu</label>
                            </div>
                            <h5 className="text-danger text-center mt-1">{error}</h5>
                            <div className="d-flex gap-2 justify-content-between">
                                <Link className="link mb-3" to="/QuenMatKhau">Quên mật khẩu?</Link>
                                <div className="form-check form-check-reverse">
                                    <input className="form-check-input rounded-0" type="checkbox" value=""
                                           onChange={(e) => showpassonchange(e)} id="ShowPassword"/>
                                    <label className="form-check-label" htmlFor="ShowPassword">Hiện mật khẩu</label>
                                </div>
                            </div>
                            <div className="form-check">
                                <input className="form-check-input rounded-0" type="checkbox" value=""
                                       id="flexCheckDefault" {...register("remember")} />
                                <label className="form-check-label" htmlFor="flexCheckDefault">Nhớ lượt đăng nhập</label>
                            </div>
                        </div>
                        <button
                            className={`btn btn-outline-success rounded-5 fw-bolder border-3 w-100 mt-2  ${loading ? "disabled" : ""}`}
                            type="submit">{loading ?
                            <>
                                <span className="spinner-grow spinner-grow-sm ms-1" role="status"
                                      aria-hidden="true"></span>
                                <span className="spinner-grow spinner-grow-sm ms-1" role="status"
                                      aria-hidden="true"></span>
                                <span className="spinner-grow spinner-grow-sm ms-1" role="status"
                                      aria-hidden="true"></span>
                            </> : <>
                                Đăng nhập
                            </>
                        }</button>
                        <Link className="link mb-3" to="/Register">Đăng ký tài khoản</Link>
                    </form>
                </div>
        </>
    )
}