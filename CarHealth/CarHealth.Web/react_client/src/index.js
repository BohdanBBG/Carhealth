
import React from 'react'
import { render } from 'react-dom'
import App from './components/app'

import HomePage from './components/routes/homePage/HomePage.js'
import CarPartsPage from './components/routes/carPartsPage/CarPartsPage.js'
import CarsControlPage from './components/routes/carsControlPage/CarsControlPage.js'
import Admin from './components/routes/admin/Admin.js'
import LogOut from './components/routes/logOut/LogOut.js'


const routes =
  [
    {
      "route": "/",
      "name": "Home",
      "component": HomePage,
      "disabled": false,
      
    },
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
    },
    {
      "route": "/admin",
      "name": "Admin",
      "component": Admin,
      "disabled": false,
    }
  ];


render(<App routes={routes}/>, document.getElementById('root'))


