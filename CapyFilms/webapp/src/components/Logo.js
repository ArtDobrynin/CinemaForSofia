import React from "react";
import Capybara from "../img/capybara.svg"

const Logo = (widthLogo) =>{
    return(
    <p className="logo_main">
        <img src={Capybara} alt="Capybara" className="logo"/>
    </p>)
}

export default Logo