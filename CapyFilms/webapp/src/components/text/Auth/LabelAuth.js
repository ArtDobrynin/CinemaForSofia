import React from "react";

class LabelAuth extends React.Component{
    render(){
        return(
            <div>
                <label className="label_auth">{this.props.text}</label>
            </div>
        )
    }
}

export default LabelAuth