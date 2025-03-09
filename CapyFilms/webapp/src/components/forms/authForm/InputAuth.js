import React, { useState, forwardRef, useImperativeHandle} from "react";

const InputAuth = forwardRef(({valueInput, index, typeInput}, ref) => {
    const [inputValue, setInputValue] = useState('');

    const handleChange = (event) => {
        const newValue = event.target.value;
        valueInput(index, newValue); 

        setInputValue(newValue);
    };

    useImperativeHandle(ref, () => ({
        resetInput() {
        setInputValue('');
        }
    }));

    return(
        <input className="input_auth" value={inputValue} onChange={handleChange} type={typeInput}></input>
    )
})

export default InputAuth