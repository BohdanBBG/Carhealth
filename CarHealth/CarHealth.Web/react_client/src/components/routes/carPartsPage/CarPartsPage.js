import React, { Component, PureComponent } from 'react';
import ReactPaginate from "react-paginate";

import 'bootstrap/dist/css/bootstrap.css';
import '../../../styles/css/paginationStyle.css'

import CarPartList from './CarPartList.js';
import ModalWindow from "../../forms/ModalWindow.js";
import Form from "../../forms/Form.js";

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
    {
        "id": "1",
        "title": "ABS Car Control Unit That Use For Test",
        "ride": "100000",
        "price": "20000",
        "date": "2020.02.02",
        "recommendedRide": "200000"
    }
]

const PER_PAGE = 12;

class CarPartsPage extends Component {

    constructor(props) {
        super(props);

        this.state = {
            data: [],
            currentPageData: [],
            currentPage: 0,
            pageCount: 0,
            limit: 12,
            offset: 0
        }
    }

    handlePageClick = ({ selected: selectedPage }) => {

        this.setState({
            data: carPartList,
            pageCount: Math.ceil(carPartList.length / PER_PAGE)
        }, () => {
            this.setState({
                currentPage: selectedPage,
                offset: selectedPage * PER_PAGE,
            }, () => {
                this.setState({ currentPageData: this.state.data.slice(this.state.offset, this.state.offset + PER_PAGE) })
            });
        });
    }

    render() {

        return (

            <div className="container-fluid pl-0 pt-4  ">

                <div className="col-3 ml-4 pb-4">
                    <input type="text" className="form-control" placeholder="Search for..." />
                </div>


                <div className="fixed-bottom ml-4" style={{ left: '90%', right: '50%', bottom: '8%' }}>
                    <a className="btn btn-lg btn-success btn-circle" data-toggle="modal" href="#carItemAddModalWindow">
                        <i className="fas fa-plus"></i>
                    </a>
                </div>

                <CarPartList data={this.state.currentPageData}></CarPartList>

                <div className="navigation-buttons pl-5 d-flex flex-row">
                    <ReactPaginate
                        initialPage={0}
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
                            <input type="date" min="1900-01-01" max="2100-12-31" defaultValue="2020-11-26"
                                className="form-control" />

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