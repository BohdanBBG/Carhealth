
import React, { Component, PureComponent } from 'react'
import 'bootstrap/dist/css/bootstrap.css'

import CarList from './carList/CarList.js'

const carList = [
    {
       "title": "Default loved user`s car",
       "ride": "1000000",
       "isCurrent": true,
    },
    {
        "title": "Car1",
        "ride": "100000",
        "isCurrent": false,
     },
     {
        "title": "Some car",
        "ride": "10000",
        "isCurrent": false,
     },
     {
        "title": "One of cars",
        "ride": "1000",
        "isCurrent": false,
     },
   
]

class CarsControlPage extends Component {

    render() {

        return (
            <div className="container-fluid pl-0 pt-4  ">

                <CarList data={carList}></CarList>              

            </div>
        );
    }

}

export default CarsControlPage