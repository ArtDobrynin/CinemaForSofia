import React from "react";
import HomeIcon from "../../../img/homeIcons.svg";
import RandomIcon from "../../../img/randomIcons.svg";
import UserIcon from "../../../img/userIcons.svg";
import Logo from "../../Logo";

function Nav({ onViewChange, activeView }) {
    return (
        <aside className="nav_panel">
            <Logo />
            <div className="main_nav_elements">
                <img
                    className="main_icons home_icons"
                    alt="Home"
                    src={HomeIcon}
                    onClick={() => onViewChange("home")}
                    style={{ opacity: activeView === "home" ? 1 : 0.5 }}
                />
                <img
                    className="main_icons random_icons"
                    alt="Random"
                    src={RandomIcon}
                    onClick={() => onViewChange("random")}
                    style={{ opacity: activeView === "random" ? 1 : 0.5 }}
                />
                <img
                    className="main_icons user_icons"
                    alt="User"
                    src={UserIcon}
                    onClick={() => onViewChange("personal")}
                    style={{ opacity: activeView === "personal" ? 1 : 0.5 }}
                />
            </div>
        </aside>
    );
}

export default Nav;