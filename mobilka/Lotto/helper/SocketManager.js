import React from "react"
//import signalr from 'react-native-signalr';
import * as signalR from "@aspnet/signalr"


export default class SocketManager {
    connection
    static LOGIN = "login"

    onChangeListener = (type, message) => {}

    constructor(props) {
        this.connection = new signalR.HubConnectionBuilder()
        .withUrl(`http://hackyeah.azurewebsites.net/game/hub?userId=${props.id}`)
        .configureLogging(signalR.LogLevel.Information)
        .build();
        
        this.connection.start().then(()=>{
            this.initListeners()
        })
        
    }

    setListener(listener) {
        this.onChangeListener = listener
    }

    initListeners = () => {
        this.connection.on("NextRollData", (message) => { 
            this.onChangeListener("NextRollData",message)
        })

        this.connection.on("HasWonRoll", (message) => { 
            this.onChangeListener("HasWonRoll",message)
        })

        this.connection.on("NewRollAvailable", () => { 
            this.connection.invoke("InvokeNextRollData").catch(err => console.log(err));
        })

        this.connection.invoke("InvokeNextRollData").catch(err => console.log(err));
    }

    //InvokeBuyTicket
    sendMessage = (eventName, body) => {
        this.connection.invoke(eventName, body) //Example: sendMessage('login', JSON.stringify({token: tokenText}))
    }
}
