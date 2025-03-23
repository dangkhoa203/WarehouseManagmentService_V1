import {useEffect, useState} from 'react';
import {useNavigate} from 'react-router-dom';
import './App.css';
import "bootstrap/dist/css/bootstrap.min.css"
import 'bootstrap-icons/font/bootstrap-icons.css'
import RouteComponent from './Component/RouteComponent';
import NavBar from './Component/NavBar';

function App() {

    const [user, setUser] = useState({
        userName: "",
        userFullName: "",
        userEmail: "",
        userId: "default",
        isLogged: false
    })
    const [loadingScreen, setLoadingScreen] = useState(false);
    const escFunction=(event)=>{
        if (event.key === "Escape") {
            navigate(-1)
        }
    }
    const componentDidMount=()=>{
        document.addEventListener("keydown", escFunction, false);
    }
    async function getInfo() {
        try {
            setLoadingScreen(true);
            const response = await fetch('https://localhost:7075/api/Account/Account', {
                headers: {'Content-Type': 'application/json'},
                credentials: 'include',
            });
            if (!response.ok) {
                const text = await response.text();
                throw Error(text);
            }
            const content = await response.json();
            setUser(content);
        } catch (er) {
            setUser({
                userName: "",
                userFullName: "",
                userEmail: "",
                userId: "",
                isLogged: false
            })
        } finally {
            setLoadingScreen(false);
        }

    }
    const navigate = useNavigate()

    useEffect(() => {
        getInfo()
        componentDidMount()
    }, [])

    return (
        <>

            {loadingScreen ?
                <div className="loadingscreen">
                    <div className="dot"></div>
                    <span className="text">Loading</span>
                </div> :
                <div>
                    {user.isLogged ?
                        <NavBar navigate={navigate} getInfo={getInfo} user={user} setUser={setUser}></NavBar> : <></>}
                    <div className='App '>
                        <RouteComponent navigate={navigate} user={user} setUser={setUser} getInfo={getInfo}/>
                    </div>
                </div>
            }
        </>
    );
}

export default App;