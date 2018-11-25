import React from 'react';

const url = "http://hackyeah.azurewebsites.net"

export default class ApiManager {

    static login(email, password){
        return fetch(url + '/auth/login', {
        method: 'POST',
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            email: email,
            password: password,
        }),
        }).then((response) => response.json())
        .then((responseJson) => {
          return responseJson;
        }).catch((error) => {
            console.error(error);
            return undefined
          });
    }

    static register(email, password){
        return fetch(url + '/auth/register', {
        method: 'POST',
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            email: email,
            password: password,
        }),
        }).then((response) => response.json())
        .then((responseJson) => {
          return responseJson;
        }).catch((error) => {
            console.error(error);
            return undefined
          });
    }
}