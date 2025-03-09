import React, { useRef, useState } from "react";
import Logo from "../../Logo";
import AuthButton from "../../buttons/Auth/AuthButton";
import InputAuth from "./InputAuth";
import RegisterButton from "../../buttons/Auth/RegisterButton";
import LabelAuth from "../../text/Auth/LabelAuth";
import axios from "axios";
import { useAuth } from '../../AuthProvider';


function Auth(){
    const childRef = useRef();
    let [widthButton, setWidthButton] = useState(120);
    let [registerForm, setRegisterForm] = useState(false);
    let [email, setEmail] = useState("");
    let [nickName, setLogin] = useState("");
    let [password, setPassword] = useState("");
    const { login } = useAuth();

    const get_value = (index, value) => {
        if(index === "email") setEmail(value);
        if(index === "login") setLogin(value);
        if(index ==="password") setPassword(value);
    }

    const click_register = () => {
        setRegisterForm(!registerForm) 
        setWidthButton(275);
        handleReset();

        const request = {
            Email: email,
            Login: nickName,
            Password: password
        }

        axios.post('https://localhost:7294/api/v1/User/register', request, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                "Access-Control-Allow-Origin": "*"
            }
        })
        .then((response) => {
            if (response.data.data.token) {
                login(response.data.data.token); // Сохраняем токен
            }
            console.log(response.data);
        })
        .catch((error) => {
          console.log(error);
        })
    }

    const handleReset = () => {
      childRef.current.resetInput()
    };

    const click_register_link = () => {
            setRegisterForm(!registerForm);
            setWidthButton(275);
            handleReset();
    }

    const click_login = () => {
        const request = {
            Login: nickName,
            Password: password
        }

        axios.post('https://localhost:7294/api/v1/User', request, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                "Access-Control-Allow-Origin": "*"
            }
        })
        .then((response) => {
            if (response.data.data.token) {
                login(response.data.data.token); // Сохраняем токен
            }
            console.log(response.data);
        })
        .catch((error) => {
          console.log(error);
        })
    }

    return(
        <div className="auth-frame">
            <Logo />
            <LabelAuth text={registerForm ? "Регистрация" : "Войти"}/>
            <div className="input_form_auth">
                {registerForm && <LabelAuth text="Email"/>}
                {registerForm && <InputAuth ref={childRef} index={"email"} valueInput={get_value} typeInput={"text"}/>}
                <LabelAuth text="Логин"/>
                <InputAuth ref={childRef} index={"login"} valueInput={get_value} typeInput={"text"}/>
                <LabelAuth text="Пароль"/>
                <InputAuth ref={childRef} index={"password"} valueInput={get_value} typeInput={"password"}/>
            </div>
            <AuthButton  widthButton={widthButton} text={registerForm ? "Регистрация" : "Войти"} onClick={registerForm ? click_register : click_login}/>

            {!registerForm && <RegisterButton onClick={ click_register_link }/>}
        </div>)
}

export default Auth