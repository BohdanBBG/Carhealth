
import React, { Component, PureComponent } from 'react'
import 'bootstrap/dist/css/bootstrap.css'

import ListItem from './ListItem.js'
import ModalWindow from "../../forms/ModalWindow.js"
import Form from "../../forms/Form.js"


class CarPartList extends Component {

    state = {
        currentItem: null
    }

    componentDidMount() {
        console.log("CarParts----", ' did mount');
    }

    render() {

        const listElements = this.props.data.map((item, index) =>
            <ListItem
                carItem={item}
                key={index}
                onEditButtonClick={this.getItemFromEditButton.bind(this, item)}>

            </ListItem>);


        return (
            <div className="container-fluid " >

                <div className="container-fluid  d-flex flex-row flex-wrap pb-2  " >

                    {listElements}
                    {listElements}

                    {listElements}
                    {listElements}

                </div>

                
                <ModalWindow title="Edit car item" id="carItemEditModalWindow" inCenter={true}>
                    <Form id="carItemEditForm" >
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
                            <label className="mr-sm-2">Price:</label>
                            <input
                                name="price"
                                defaultValue={`${this.state.currentItem === null ? "" : String(this.state.currentItem.price).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ') }`}
                                type="text"
                                className="form-control input-lg"
                                placeholder="Price"
                                required
                                data-parsley-type="digits"
                                data-parsley-trigger="focusin focusout " />
                        </div>

                        <div className="form-group">
                            <label className="mr-sm-2">Date:</label>
                            <input
                                name="date"
                                type="text"
                                defaultValue={`${this.state.currentItem === null ? "" : this.state.currentItem.date}`}
                                className="form-control input-lg"
                                placeholder="Date"
                                required
                                data-parsley-length="[6, 10]"
                                data-parsley-trigger="keyup" />
                        </div>

                        <div className="form-group form-check  ml-2 ">
                            <label className="form-check-label">
                                <input
                                    name="isReplaced"
                                    className="form-check-input"
                                    type="checkbox" />
                                  Is replaced
                             </label>
                        </div>

                    </Form>
                </ModalWindow>

                <ModalWindow title={`${this.state.currentItem === null ? "" : " Delete " + this.state.currentItem.title + " ?"}`} id="carItemDeleteModalWindow" inCenter={false}>
                    <Form id="carItemDeleteForm">
                    </Form>
                </ModalWindow>


            </div>);
    }

    getItemFromEditButton = (item) => this.setState({
        currentItem: item
    })



}

export default CarPartList