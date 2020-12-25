
import React, { Component } from 'react'
import 'bootstrap/dist/css/bootstrap.css'

import { ToastContainer, toast } from 'react-toastify';

import { connect } from 'react-redux';

import ListItem from './ListItem.js'
import ModalWindow from "../../forms/ModalWindow.js"
import Form from "../../forms/Form.js"

import { ApiService } from '../../../services/ApiService.js';


class CarPartList extends Component {

    constructor(props) {
        super(props);

        this.state = {
            currentItem: null
        }
        this.apiService = new ApiService();
    }

    componentDidMount() {
        console.log("CarParts----", ' did mount');
    }

    deleteItem = (url, itemId) => {

        this.apiService.deleteRequest(url)
            .then(() => {
                this.getSuccessToast("Car item successfully deleted");
                this.props.onItemListChange(this.props.data.filter(function (value) {
                    return value.id !== itemId;
                }));
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

        const listElements = this.props.data.map((item) =>
            <ListItem
                carItem={item}
                key={item.id}
                onEditButtonClick={this.getItemFromEditButton.bind(this, item)}>

            </ListItem>);


        return (
            <div className="container-fluid " >

                <div className="container-fluid  d-flex flex-row flex-wrap pb-2  " >
                    {listElements}
                </div>


                <ModalWindow title="Edit car item" id="carItemEditModalWindow" inCenter={true}>
                    <Form id="carItemEditForm" >
                        <div className="form-group">
                            <label>Detail name</label>
                            <input
                                name="name"
                                defaultValue={`${this.state.currentItem === null ? "" : this.state.currentItem.name}`}
                                type="text"
                                className="form-control input-lg"
                                placeholder="Name"
                            />
                        </div>

                        <div className="form-group">
                            <label className="mr-sm-2">Price:</label>
                            <input
                                name="priceOfDetail"
                                defaultValue={`${this.state.currentItem === null ? "" : String(this.state.currentItem.priceOfDetail).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ')}`}
                                type="text"
                                className="form-control input-lg"
                                placeholder="Detail price"
                            />

                        </div>

                        <div className="form-group">
                            <label className="mr-sm-2">Replaced:</label>
                            <input
                                name="replaced"
                                type="text"
                                defaultValue={`${this.state.currentItem === null ? "" : this.state.currentItem.replaced.slice(0, 10)}`}
                                className="form-control input-lg"
                                placeholder="Replaced"
                            />
                        </div>

                        <div className="form-group">
                            <label className="mr-sm-2">Replace at:</label>
                            <input
                                name="replaceAt"
                                type="text"
                                defaultValue={`${this.state.currentItem === null ? "" : this.state.currentItem.replaceAt.slice(0, 10)}`}
                                className="form-control input-lg"
                                placeholder="Replace at"
                            />
                        </div>

                        <div className="form-group form-check  ml-2 ">
                            <label className="form-check-label">
                                <input
                                    name="isTotalRideChanged"
                                    className="form-check-input"
                                    type="checkbox" />
                                  Is replaced
                             </label>
                        </div>

                    </Form>
                </ModalWindow>

                <ModalWindow title={`${this.state.currentItem === null ? "" : " Delete " + this.state.currentItem.name + " ?"}`} id="carItemDeleteModalWindow" inCenter={false}>

                    <div className="row justify-content-end mr-2">
                        <button type="button" className="btn btn-secondary col-2" data-dismiss="modal">Close</button>
                        <button type="submit" className="btn btn-primary col-2 ml-2"
                            onClick={(event) => {
                                event.stopPropagation();

                                this.deleteItem(`${this.props.appConfig.urls.api}/api/Cars/delete/caritem?detailId=${this.state.currentItem?.id}`, this.state.currentItem?.id);
                            }}
                            data-dismiss="modal">
                            Accept
                            </button>
                    </div>
                </ModalWindow>

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
            </div>);
    }

    getItemFromEditButton = (item) => this.setState({
        currentItem: item
    })

}

const putStateToProps = (state) => {

    return {
        appConfig: state.config.appConfig,
    };
}

export default connect(putStateToProps)(CarPartList);
