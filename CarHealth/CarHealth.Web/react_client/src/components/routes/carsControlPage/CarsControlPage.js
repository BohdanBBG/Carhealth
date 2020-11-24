
import { MDBDataTableV5 } from 'mdbreact';
import 'bootstrap/dist/css/bootstrap.css';

import React, { Component, PureComponent } from 'react';

import ModalWindow from "../../forms/ModalWindow.js"
import Form from "../../forms/Form.js"
import RowControllButtons from "../carsControlPage/carList/RowControllButtons.js"



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
        label: 'ID',
        field: 'id',
        sort: 'disabled'
    },
    {
        label: 'Title',
        field: 'title',
        attributes: {
            'aria-controls': 'DataTable',
            'aria-label': 'title',
        },
    },
    {
        label: 'Ride',
        field: 'ride',
        sort: 'disabled'
    },
    {
        label: 'IsCurrent',
        field: 'isCurrent',
    },
    {
        label: 'Control',
        field: 'control',
        sort: 'disabled',
    },
];


class CarsControlPage extends Component {

    constructor(props) {
        super(props);

        this.getItemFromEditButton = this.getItemFromEditButton.bind(this);

        this.state = {
            data:
            {
                columns: columns,
                rows: carList
            },
            currentItem: null
        }
    }

    render() {

        this.state.data.rows.map((item, index) => {
            item.ride = String(item.ride).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ');

            item.isCurrent = item.isCurrent.toString();

            item["control"] = <RowControllButtons
                key={1}
                index={index + 1}
                onEditButtonClick={this.getItemFromEditButton.bind(this, item)}
            />
        }
        );

        return (
            <div className="card-body">
                <MDBDataTableV5 className="table-striped text-center "
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
                                    checked={this.state.currentItem == null ? false :(this.state.currentItem.isCurrent == "true")}
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

            </div>
        );
    }

    getItemFromEditButton = (item) => {
        this.setState({
            currentItem: item
        })
    }

}

export default CarsControlPage