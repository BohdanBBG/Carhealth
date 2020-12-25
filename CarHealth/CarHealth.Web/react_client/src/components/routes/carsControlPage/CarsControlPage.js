
import { MDBDataTableV5 } from 'mdbreact';
import 'bootstrap/dist/css/bootstrap.css';

import React, { Component } from 'react';

import ModalWindow from "../../forms/ModalWindow.js";
import Form from "../../forms/Form.js"
import RowControllButtons from "../carsControlPage/carList/RowControllButtons.js";

import { ApiService } from '../../../services/ApiService.js';

import { connect } from 'react-redux';

import Loader from '../../loader.js'
import { ToastContainer, toast } from 'react-toastify';


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

    componentDidMount() {

        this.getUserCars();

        console.log("CarsControlPage ----", ' did mount');
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

    deleteCar = (url, itemId) => {

        this.apiService.deleteRequest(url)
            .then(() => {

                this.getSuccessToast("Car successfully deleted");
                this.setState({
                    data: {
                        columns: columns,
                        rows: this.state.data.rows.filter(function (value) {
                            return value.id !== itemId;
                        })
                    }
                });

            }
            ).catch(error => {
                console.error("----CarPartsPage.CarPartList Rejected:", error);
                this.getErrorToast(error);
            })
    }

    getSuccessToast = (message) => {
        toast.success(message, {
        });
    }

    getErrorToast = (message) => {
        toast.error(message, {
        });
    }

    getInfoToast = (message) => {
        toast.info(message, {
        });
    }

    getWarnToast = (message) => {
        toast.warn(message, {
        });
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
            <>
                {this.state.data.rows.length === 0
                    ? <Loader></Loader>
                    : <>
                        <div className="card-body ">

                            <div className="fixed-bottom ml-4" style={{ left: '90%', right: '50%', bottom: '8%' }}>
                                <a className="btn btn-lg btn-success btn-circle" data-toggle="modal" href="#carEntityAddModalWindow">
                                    <i className="fas fa-plus"></i>
                                </a>
                            </div>

                            <MDBDataTableV5 className="table-striped p-4"
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
                                        <label className="mr-sm-2">Car name:</label>
                                        <input
                                            name="carEntityName"
                                            defaultValue={`${this.state.currentItem === null ? "" : this.state.currentItem.carEntityName}`}
                                            type="text"
                                            className="form-control input-lg"
                                            placeholder="Car name"
                                        />
                                    </div>

                                    <div className="form-group">
                                        <label className="mr-sm-2">Miliage:</label>
                                        <input
                                            name="totalRide"
                                            type="text"
                                            defaultValue={`${this.state.currentItem === null ? "" : String(this.state.currentItem.totalRide).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ')}`}
                                            className="form-control input-lg"
                                            placeholder="Miliage"
                                        />
                                    </div>
                                </Form>
                            </ModalWindow>

                            <ModalWindow title={`${this.state.currentItem === null ? "" : " Delete " + this.state.currentItem.carEntityName + " ?"}`} id="carEntityDeleteModalWindow" inCenter={false}>
                                <div className="row justify-content-end mr-2">
                                    <button type="button" className="btn btn-secondary col-2" data-dismiss="modal">Close</button>
                                    <button type="submit" className="btn btn-primary col-2 ml-2"
                                        onClick={(event) => {
                                            event.stopPropagation();

                                            this.deleteCar(`${this.props.appConfig.urls.api}/api/Cars/delete/car?carEntityId=${this.state.currentItem?.id}`, this.state.currentItem?.id);
                                        }}
                                        data-dismiss="modal">
                                        Accept
                            </button>
                                </div>
                            </ModalWindow>

                            <ModalWindow title="Add car item" id="carEntityAddModalWindow" inCenter={true}>
                                <Form id="carEntityAddForm">
                                    <div className="form-group">
                                        <label className="mr-sm-2">Car name:</label>
                                        <input
                                            name="carEntityName"
                                            type="text"
                                            className="form-control input-lg"
                                            placeholder="Car name"
                                        />
                                    </div>

                                    <div className="form-group">
                                        <label className="mr-sm-2">Miliage:</label>
                                        <input
                                            name="totalRide"
                                            type="text"
                                            className="form-control input-lg"
                                            placeholder="Miliage"
                                        />
                                    </div>
                                </Form>
                            </ModalWindow>

                        </div>
                        <ToastContainer
                            position="top-right"
                            autoClose={5000}
                            hideProgressBar={false}
                            newestOnTop
                            closeOnClick
                            rtl={false}
                            pauseOnFocusLoss
                            draggable
                            pauseOnHover
                        />
                    </>
                }
            </>
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
