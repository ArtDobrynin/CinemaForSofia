import React  from "react";
import Auth from "./components/forms/authForm/AuthForm"
import Main from "./components/forms/mainForm/MainForm";
import  { AuthProvider, useAuth } from '../src/components/AuthProvider';

const App = () => {
    const { token } = useAuth(); // Получаем токен из контекста

    return(
    <div className="main_frame">
        {token ? <Main /> : <Auth />}
    </div>)
}

// Обертываем приложение в AuthProvider для предоставления контекста
const AppWrapper = () => (
    <AuthProvider>
      <App />
    </AuthProvider>
  );

  export default AppWrapper;