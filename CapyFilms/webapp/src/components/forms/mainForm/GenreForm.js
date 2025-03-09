import React, { useState } from "react";

const Genre = ({ onGenreChange }) =>{
    const [genres] = useState(["Новинки", "Приключения", "Мелодрама", "Ужасы", "Семейный", "Криминал"]);

    const handleGenreClick = (genre) => {
        onGenreChange(genre); // Передаем выбранный жанр в родительский компонент
        console.log(genre)
    };

    return(
    <div className="genre_panel">
        <ul className="genre_list">
            {(genres.map((genre, index) => (
                <li key={index} className="genre_element">
                    <button className="genre" onClick={() => handleGenreClick(genre)}>{genre}</button>
                </li>
            )))}
        </ul>
    </div>)
}

export default Genre