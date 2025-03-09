import { Component } from "react";

  class AuthButton extends Component{
    render(){
        return(
            <div>
                <button className="button_yandx" onClick={this.props.onClick} min-width={this.props.widthButton}>{this.props.text}</button>
            </div>)
    }
  }

export default AuthButton