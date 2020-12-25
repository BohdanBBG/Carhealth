
import { MDBDataTableV5 } from 'mdbreact';
import 'bootstrap/dist/css/bootstrap.css';

import React, { Component } from 'react';

import ModalWindow from "../../forms/ModalWindow.js";
import Form from "../../forms/Form.js"
import RowControllButtons from "../carsControlPage/carList/RowControllButtons.js";

import { ApiService } from '../../../services/ApiService.js';

import { connect } from 'react-redux';

import Loader from '../../loader.js'

const carList = [
    {
        id: "1",
        title: "Default loved user`s car",
        ride: "1000",
        isCurrent: true
    },
    {
        id: "2",
        title: "Car1",
        ride: "10000",
        isCurrent: false,
    },
    {
        id: "3",
        title: "Some car",
        ride: "10000",
        isCurrent: false,
    },
    {
        id: "4",
        title: "One of cars",
        ride: "100000",
        isCurrent: false,
    },
    {
        id: "2",
        title: "Car1",
        ride: "1000000",
        isCurrent: false,
    },
    {
        id: "2",
        title: "Car1",
        ride: "10000",
        isCurrent: false,
    },
    {
        id: "3",
        title: "Some car",
        ride: "10000",
        isCurrent: false,
    },
    {
        id: "4",
        title: "One of cars",
        ride: "100000",
        isCurrent: false,
    },
    {
        id: "2",
        title: "Car1",
        ride: "1000000",
        isCurrent: false,
    },
    {
        id: "2",
        title: "Car1",
        ride: "10000",
        isCurrent: false,
    },
    {
        id: "3",
        title: "Some car",
        ride: "10000",
        isCurrent: false,
    },
    {
        id: "4",
        title: "One of cars",
        ride: "100000",
        isCurrent: false,
    },
    {
        id: "2",
        title: "Car1",
        ride: "1000000",
        isCurrent: false,
    },
];



const columns = [

    {
        label: 'Car name',
        field: 'carEntityName',
        attributes: {
        },
    },
    {
        label: 'Miliage',
        field: 'totalRide',
        sort: 'disabled',
        attributes: {
        },
    },
    {
        label: 'Is current',
        field: 'isDefault',
        attributes: {
        },
    },
    {
        label: 'Control',
        field: 'control',
        sort: 'disabled',
        attributes: {
        },
    },
];


class CarsControlPage extends Component {

    constructor(props) {
        super(props);

        this.state = {
            data:
            {
                columns: columns,
                rows: []
            },
            currentItem: null,
        }

        this.apiService = new ApiService();

        this.getItemFromEditButton = this.getItemFromEditButton.bind(this);

    }


    getTestData = () => {

        console.log('CarsControlPage.js 0-0-0-0-0-0 ', this.props);

        this.apiService.getRequest(`${this.props.appConfig.urls.api}/test/ping`)
            .then(
                response => {

                    console.log("----CarsControlPage.getTestRequest:", response);
                })
            .catch(error => { console.error("----CarsControlPage.getTestRequest Rejected:", error) });
    }

    getUserCars = () => {

        this.apiService.getRequest(`${this.props.appConfig.urls.api}/api/Cars/allUsersCars`)
            .then(
                response => {
                    response.map((item => {
                        item.totalRide = String(item.totalRide).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ');

                        item["isDefault"] = item.isDefault.toString();


                        item["control"] = <RowControllButtons
                            key={item.id}
                            index={item.id}
                            onEditButtonClick={this.getItemFromEditButton.bind(this, item)}
                        />
                    }));

                    this.setState({
                        data: {
                            columns: columns,
                            rows: response

                        }
                    });
                    console.log("----CarsControlPage.getUserCars: ", response)
                })
            .catch(error => { console.error("----CarsControlPage.getTestRequest Rejected:", error) });
    }

    componentDidMount() {

        // this.getTestData();
        this.getUserCars();

        console.log("CarsControlPage ----", ' did mount');
    }

    render() {

        console.log("----Caaaaaaaaaaaaaaaaaaaaaaaaa: ", this.state.data);
        // this.state.data.rows.map((item, index) => {
        //     item.carsTotalRide = String(item.carsTotalRide).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ');

        //     item.isCurrent = item.isCurrent.toString();

        //     item["control"] = <RowControllButtons
        //         key={index}
        //         index={index}
        //         onEditButtonClick={this.getItemFromEditButton.bind(this, item)}
        //     />
        // }
        // );

        return (
            <div className="card-body ">

                <div className="fixed-bottom ml-4" style={{ left: '90%', right: '50%', bottom: '8%' }}>
                    <a className="btn btn-lg btn-success btn-circle" data-toggle="modal" href="#carEntityAddModalWindow">
                        <i className="fas fa-plus"></i>
                    </a>
                </div>

                <MDBDataTableV5 className="table-striped text-center text-dark"
                    hover
                    entriesOptions={[5, 10, 15]}
                    theadColor="thead-dark"
                    entries={6}
                    data={this.state.data}
                    searchTop
                    pagingTop
                    barReverse
                    searchBottom={false}
                    fullPagination
                />

                <ModalWindow title="Edit car item" id="carEntityEditModalWindow" inCenter={true}>
                    <Form id="carEntityEditForm" >
                        <div className="form-group">
                            <label className="mr-sm-2">Title:</label>
                            <input
                                name="title"
                                defaultValue={`${this.state.currentItem === null ? "" : this.state.currentItem.title}`}
                                type="text"
                                className="form-control input-lg"
                                placeholder="Title"
                                required
                                data-parsley-type="email" />
                        </div>

                        <div className="form-group">
                            <label className="mr-sm-2">Ride:</label>
                            <input
                                name="price"
                                type="text"
                                defaultValue={`${this.state.currentItem === null ? "" : String(this.state.currentItem.ride).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ')}`}
                                className="form-control input-lg"
                                placeholder="Ride"
                                required
                                data-parsley-length="[6, 10]"
                                data-parsley-trigger="keyup" />
                        </div>


                        <div className="form-group form-check  ml-2 ">
                            <label className="form-check-label">
                                <input
                                    name="isCurrent"
                                    className="form-check-input"
                                    type="checkbox"
                                    checked={this.state.currentItem === null ? false : (this.state.currentItem.isCurrent === "true")}
                                />
                                  Is current
                             </label>
                        </div>

                    </Form>
                </ModalWindow>

                <ModalWindow title={`${this.state.currentItem === null ? "" : " Delete " + this.state.currentItem.title + " ?"}`} id="carEntityDeleteModalWindow" inCenter={false}>
                    <Form id="carEntityDeleteForm">
                    </Form>
                </ModalWindow>

                <ModalWindow title="Add car item" id="carEntityAddModalWindow" inCenter={true}>
                    <Form id="carEntityAddForm">
                        <div className="form-group">
                            <label className="mr-sm-2">Title:</label>
                            <input
                                name="title"
                                type="text"
                                className="form-control input-lg"
                                placeholder="Title"
                                required
                                data-parsley-type="email" />
                        </div>

                        <div className="form-group">
                            <label className="mr-sm-2">Ride:</label>
                            <input
                                name="price"
                                type="text"
                                className="form-control input-lg"
                                placeholder="Ride"
                                required
                                data-parsley-length="[6, 10]"
                                data-parsley-trigger="keyup" />
                        </div>


                        <div className="form-group form-check  ml-2 ">
                            <label className="form-check-label">
                                <input
                                    name="isCurrent"
                                    className="form-check-input"
                                    type="checkbox"
                                />
                                  Is current
                             </label>
                        </div>
                    </Form>
                </ModalWindow>

            </div>
        );
    }

    getItemFromEditButton = (item) => {
        this.setState({
            currentItem: item
        })
    }

}


const putStateToProps = (state) => {

    return {
        appConfig: state.config.appConfig,
    };
}

export default connect(putStateToProps)(CarsControlPage);
