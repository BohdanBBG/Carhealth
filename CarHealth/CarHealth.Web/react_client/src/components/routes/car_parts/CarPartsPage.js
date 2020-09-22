import React, { Component, PureComponent } from 'react'
import 'bootstrap/dist/css/bootstrap.css'

import CarPartList from './CarPartList.js'

const carPartList = [
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

class CarPartsPage extends Component {

    componentDidMount() {
        console.log("CarParts----", ' did mount');
    }

    render() {



        return (

            <div className="container-fluid pl-0 pt-4  ">

                <div className="navbar pl-4 ">
                    <form className="form-inline">
                        <input className="form-control mr-sm-2" type="search" placeholder="Search" aria-label="Search" />
                        <button className="btn btn-outline-info my-2 my-sm-0" type="submit">Search</button>
                    </form>
                </div>

                <CarPartList data={carPartList}></CarPartList>

                <div className="navigation-buttons pl-4 d-flex flex-row">
                    <ul className="pagination justify ">
                        <li className="page-item ">
                            <a className="page-link" href="#">Previous</a>
                        </li>
                        <li className="page-item active">
                            <a className="page-link" href="#">1</a>
                        </li>
                        <li className="page-item ">
                            <a className="page-link" href="#">2</a>
                        </li>
                        <li className="page-item">
                            <a className="page-link" href="#">3</a>
                        </li>
                        <li className="page-item">
                            <a className="page-link" href="#">Next</a>
                        </li>
                    </ul>
                    <div >
                        <button type="button" className=" btn  btn-outline-success  ml-4  ">Add</button>
                    </div>
                </div>

            </div>


        );
    }
}

export default CarPartsPage;