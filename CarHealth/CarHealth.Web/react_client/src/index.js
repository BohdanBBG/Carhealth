
import React from 'react'
import { render } from 'react-dom'
import App from './components/app'

import CarPartsPage from './components/routes/carPartsPage/CarPartsPage.js'
import CarsControlPage from './components/routes/carsControlPage/CarsControlPage.js'

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
    },
    // {
    //   "route": "/admin",
    //   "name": "Admin",
    //   "component": () => {
    //     window.location.href = 'https://localhost:5006/Users/Index';
    //     return null;
    //   },
    //   "disabled": false,
    // }
  ];


  render(<App routes={routes} />, document.getElementById('root'))


