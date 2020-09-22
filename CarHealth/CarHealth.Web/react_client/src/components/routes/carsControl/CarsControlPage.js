
import React, { Component, PureComponent } from 'react'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap';

import CarList from './carList/CarList.js'

const carList = [
    {
        "title": "ABS Car Control Unit That Use For Test",
        "ride": "100000",
        "price": "20000",
        "date": "2020.02.02",
        "recommendedRide": "200000"
    },
    {
        "title": "Car Detail",
        "ride": "1000000",
        "price": "200000",
        "date": "2020.03.03",
        "recommendedRide": "100000"
    },
    {
        "title": "Stop Lights",
        "ride": "10000",
        "price": "200000",
        "date": "2020.03.03",
        "recommendedRide": "100000"
    },
]

class CarsControlPage extends Component {

    render() {

        return (
            <div className="container-fluid pl-0 pt-4  ">

                <CarList></CarList>              


            </div>
        );
    }

}

export default CarsControlPage