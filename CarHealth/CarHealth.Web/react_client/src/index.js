
import React from 'react'
import { render } from 'react-dom'
import App from './components/app'

import CarPartsPage from './components/routes/carPartsPage/CarPartsPage.js'
import CarsControlPage from './components/routes/carsControlPage/CarsControlPage.js'

import HttpUtil from './styles/js/HttpUtil.js'

const httphelper = new HttpUtil();

const routes =
  [
    {
      "route": "/carParts",
      "name": "Car parts",
      "component": CarPartsPage,
      "disabled": false,
    },
    {
      "route": "/carsControl",
      "name": "Cars control",
      "component": CarsControlPage,
      "disabled": false,
    }
  ];

httphelper.httpGet("http://localhost:5003/config").then(
  response => {
    routes.push({
      "route": "/admin",
      "name": "Admin",
      "component": () => {
        window.location.href = `${response.urls.identity}/Users/Index`;
        return null;
      },
      "disabled": false,
    });
    
    render(<App routes={routes} appConfig={response} />, document.getElementById('root'));
  },
  error => alert(`Rejected: ${error}`)
);

