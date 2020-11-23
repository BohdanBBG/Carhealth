import React, { Component, PureComponent } from 'react'
import 'bootstrap/dist/css/bootstrap.css'

import CarPartList from './CarPartList.js'
import ModalWindow from "../../forms/ModalWindow.js"
import Form from "../../forms/Form.js"

const carPartList = [
    {
        "id": "1",
        "title": "ABS Car Control Unit That Use For Test",
        "ride": "100000",
        "price": "20000",
        "date": "2020.02.02",
        "recommendedRide": "200000"
    },
    {
        "id": "2",
        "title": "Car Detail",
        "ride": "1000000",
        "price": "200000",
        "date": "2020.03.03",
        "recommendedRide": "100000"
    },
    {
        "id": "3",
        "title": "Stop Lights",
        "ride": "10000",
        "price": "200000",
        "date": "2020.03.03",
        "recommendedRide": "100000"
    },
]

class CarPartsPage extends Component {

    componentDidMount() {
        console.log("CarPartsPage----", ' did mount');
    }

    render() {



        return (

            <div className="container-fluid pl-0 pt-4  ">

                <div className="col-3 ml-4">
                        <input type="text" className="form-control" placeholder="Search for..." />
                </div>

                <CarPartList data={carPartList}></CarPartList>

                <div className="navigation-buttons pl-5 d-flex flex-row">
                    <ul className="pagination justify ">
                        <li className="page-item ">
                            <a className="page-link" href="#">&laquo;</a>
                        </li>

                        <li className="page-item ">
                            <a className="page-link" aria-disabled="true" >1</a>
                        </li>

                        <li className="page-item">
                            <a className="page-link" href="#">&raquo;</a>
                        </li>
                    </ul>
                    <div >
                        <button type="button" className=" btn  btn-outline-success  ml-4" data-toggle="modal" href="#carItemAddModalWindow">Add</button>
                    </div>
                </div>


                <ModalWindow title="Add new car item" id="carItemAddModalWindow" inCenter={true}>
                    <Form id="carItemAddForm">
                        <div className="form-group">
                            <input
                                name="title"
                                type="text"
                                className="form-control input-lg"
                                placeholder="Title"
                                required
                                data-parsley-type="email" />
                        </div>

                        <div className="form-group">
                            <input
                                name="price"
                                type="text"
                                className="form-control input-lg"
                                placeholder="Price"
                                required
                                data-parsley-length="[6, 10]"
                                data-parsley-trigger="keyup" />
                        </div>

                        <div className="form-group">
                            <input
                                name="date"
                                type="text"
                                className="form-control input-lg"
                                placeholder="Date"
                                required
                                data-parsley-length="[6, 10]"
                                data-parsley-trigger="keyup" />
                        </div>

                        <div className="form-group">
                            <input
                                name="date"
                                type="text"
                                className="form-control input-lg"
                                placeholder="Recommended ride"
                                required
                                data-parsley-length="[6, 10]"
                                data-parsley-trigger="keyup" />
                        </div>

                    </Form>
                </ModalWindow>


            </div>


        );
    }
}

export default CarPartsPage;