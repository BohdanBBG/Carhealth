
import React from 'react'
import { render } from 'react-dom'
import App from './components/app'

import Home from './components/routes/home/Home.js'
import CarPartsPage from './components/routes/car_parts/CarPartsPage.js'
import CarsControlPage from './components/routes/carsControl/CarsControlPage.js'
import Admin from './components/routes/admin/Admin.js'
import LogOut from './components/routes/logOut/LogOut.js'


const routes =
  [
    {
      "route": "/",
      "name": "Home",
      "component": Home,
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
    },
    {
      "route": "/logOut",
      "name": "LogOut",
      "component": LogOut,
      "disabled": true,
    }
  ];


render(<App routes={routes}/>, document.getElementById('root'))


