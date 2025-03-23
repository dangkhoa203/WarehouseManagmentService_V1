import {Navigate, useNavigate} from "react-router-dom";
import {useEffect} from "react";

export  default function Home (props){
    if(!props.user.isLogged && props.user.userId===""){
        return <Navigate to="/login"></Navigate>
    }
    return (
        <>
            <div className="w-100 text-center">
                <h1>Xin chào {props.user.userFullName}</h1>
            </div>
        </>
    );
}