import React from "react"
import * as ReactDOMClient from "react-dom/client"  
import "./css/App.css"
import "./css/Auth.css"
import "./css/PersonalAccount.css"
import App from "./App"
import { AuthProvider } from "./components/AuthProvider"

const root = ReactDOMClient.createRoot(document.getElementById("cypa"))
root.render(<AuthProvider><App /></AuthProvider>)