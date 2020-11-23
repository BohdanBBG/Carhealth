
import React, { Component, PureComponent } from 'react'
import 'bootstrap/dist/css/bootstrap.css'

import CarListItemRow from './CarListItemRow.js'
import ModalWindow from "../../../forms/ModalWindow.js"
import Form from "../../../forms/Form.js"

class CarList extends Component {

    state = {
        currentItem: null
    }

    componentDidMount() {
        console.log("CarList----", ' did mount');
    }

    render() {

        let carListItem = this.props.data.map((item, index) =>
            <CarListItemRow
                key={index}
                index={index + 1}
                listItem={item}
                onEditButtonClick={this.getItemFromEditButton.bind(this, item)}>

            </CarListItemRow>
        );

        return (
            <div className="container-fluid ">

                <table className="table table-hover container-fluid text-center">
                    <thead className="thead-light">
                        <tr className="row">
                            <th className="col-1" >#</th>
                            <th className="col-3">Name</th>
                            <th className="col-3">Ride</th>
                            <th className="col-2">Is current</th>
                            <th className="col-3">Additional</th>
                        </tr>
                    </thead>
                    <tbody>

                        {carListItem}

                    </tbody>
                </table>

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
                                defaultValue={`${this.state.currentItem === null ? "" : String(this.state.currentItem.ride).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ') }`}
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
                                    checked={this.state.currentItem == null? false: this.state.currentItem.isCurrent }
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

        )
    }

    getItemFromEditButton = (item) => this.setState({
        currentItem: item
    })
}

export default CarList;