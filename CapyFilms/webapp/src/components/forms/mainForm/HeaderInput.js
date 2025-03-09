import React from "react";

const Header = ({ searchQuery, onSearchChange, onSearchSubmit }) => {
    const handleInputChange = (event) => {
        onSearchChange(event.target.value); // Обновляем состояние поиска
    };

    // Функция для обработки нажатия клавиши Enter
    const handleKeyPress = (event) => {
        if (event.key === "Enter") {
            event.preventDefault(); // Предотвращаем отправку формы
            onSearchSubmit(); // Вызываем функцию для обработки поиска
        }
    };

    return (
        <div className="header_frame">
            <input
                className="search_cinema"
                value={searchQuery} // Привязываем значение поля ввода к состоянию
                onChange={handleInputChange}
                onKeyDown={handleKeyPress} // Обработчик для клавиши Enter
                autoComplete="off" // Отключаем автозаполнение
            />
        </div>
    );
};

export default Header;