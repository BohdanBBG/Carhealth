import React, { Component } from 'react';
import ReactPaginate from "react-paginate";

import 'bootstrap/dist/css/bootstrap.css';
import '../../../styles/css/paginationStyle.css'

import CarPartList from './CarPartList.js';
import ModalWindow from "../../forms/ModalWindow.js";
import Form from "../../forms/Form.js";

import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import { ApiService } from '../../../services/ApiService.js';

import { connect } from 'react-redux';

import Loader from '../../loader.js'

class CarPartsPage extends Component {

    constructor(props) {
        super(props);

        this.state = {
            currentPageData: [],
            currentPage: 0,
            pageCount: 0,
            limit: 12,
        }
        this.apiService = new ApiService();
    }

    handlePageClick = ({ selected: selectedPage = 0 }) => {

        this.apiService.getRequest(`${this.props.appConfig.urls.api}/api/cars/cardetails?offset=${selectedPage * this.state.limit}&limit=${this.state.limit}`)
            .then(
                response => {

                    this.setState({
                        pageCount: Math.ceil(response.countCarsItems / this.state.limit),
                        currentPage: selectedPage,
                        currentPageData: response.carItems
                    });

                    console.log("----CarPartsPage.getPageData: ", response)
                })
            .catch(error => {
                console.error("----CarPartsPage.getPageData Rejected:", error);
                this.getErrorToast = (error);
            });
    }

    componentDidMount() {

        this.handlePageClick({ selected: 0 });

        console.log("CarPartsPage ----", ' did mount');
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

    setStateOnItemChange = (items) => {
        this.setState({ currentPageData: items });
    }


    render() {

        return (
            <>
                {this.state.currentPageData.length === 0
                    ? <Loader></Loader>
                    : <>
                        <div className="container-fluid pl-0 pt-4  ">

                            <div className="col-3 ml-4 pb-4">
                                <input type="text" className="form-control" placeholder="Search for..." />
                            </div>


                            <div className="fixed-bottom ml-4" style={{ left: '90%', right: '50%', bottom: '8%' }}>
                                <a className="btn btn-lg btn-success btn-circle" data-toggle="modal" href="#carItemAddModalWindow">
                                    <i className="fas fa-plus"></i>
                                </a>
                            </div>

                            <CarPartList data={this.state.currentPageData} onItemListChange={this.setStateOnItemChange}></CarPartList>

                            <div className="navigation-buttons pl-5 d-flex flex-row">
                                <ReactPaginate
                                    previousLabel={"← Previous"}
                                    breakLabel={"..."}
                                    nextLabel={"Next →"}
                                    pageCount={this.state.pageCount}
                                    marginPagesDisplayed={2}
                                    pageRangeDisplayed={2}
                                    onPageChange={this.handlePageClick}
                                    containerClassName={"pagination"}
                                    previousLinkClassName={"pagination__link"}
                                    nextLinkClassName={"pagination__link"}
                                    disabledClassName={"pagination__link--disabled"}
                                    activeClassName={"pagination__link--active"}
                                />
                            </div>

                            <ModalWindow title="Add new car item" id="carItemAddModalWindow" inCenter={true}>
                                <Form id="carItemAddForm">
                                    <div className="form-group">
                                        <label>Detail name</label>
                                        <input
                                            name="name"
                                            type="text"
                                            className="form-control input-lg"
                                            placeholder="Name"
                                            required
                                        />
                                    </div>

                                    <div className="form-group">
                                        <label>Price</label>
                                        <input
                                            name="priceOfDetail"
                                            type="text"
                                            className="form-control input-lg"
                                            placeholder="Price"
                                            required
                                            data-parsley-length="[6, 10]"
                                            data-parsley-trigger="keyup" />
                                    </div>

                                    <div className="form-group">
                                        <label>Replaced</label>
                                        <input type="date" name="replaced" min="1900-01-01" max="2100-12-31" defaultValue="2020-11-26"
                                            className="form-control" />

                                    </div>

                                    <div className="form-group">
                                        <label>Replace at</label>
                                        <input type="date" name="replaceAt" min="1900-01-01" max="2100-12-31" defaultValue="2021-12-13"
                                            className="form-control" />

                                    </div>

                                    <div className="form-group">
                                        <label>Recomended replace</label>
                                        <input
                                            name="recomendedReplace"
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
                    </>
                }
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
        );
    }
}

const putStateToProps = (state) => {

    return {
        appConfig: state.config.appConfig,
    };
}

export default connect(putStateToProps)(CarPartsPage);