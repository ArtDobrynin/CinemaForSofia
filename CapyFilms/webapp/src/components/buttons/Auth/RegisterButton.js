import React from "react";

function RegisterButton({ onClick }){
    return(
        <div className="text_register_link">
            <p>Если у вас еще нет аккаунта , вы можете <button className="register_link" onClick={onClick}>Зарегистрироваться</button></p>
        </div>
    )
}

export default RegisterButton