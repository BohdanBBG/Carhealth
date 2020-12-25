
import React, { Component } from 'react'
import 'bootstrap/dist/css/bootstrap.css'
import Dropdown from 'react-bootstrap/Dropdown';
import moment from 'moment';


class ListItem extends Component {

    constructor(props) {
        super(props);

        this.state = {
            replacedDate: moment(props.carItem.replaced),
            replaceAtDate: moment(props.carItem.replaceAt)
        }
    }

    render() {
        return (
            <div className="p-2 col-md-2" >

                <div className={`card border-secondary text-center 
                ${this.props.carItem.detailMileage >= this.props.carItem.recomendedReplace ? "border-danger" : ""} 
                ${this.state.replaceAtDate.isBefore(moment().format()) ? "border-warning" : ""} `}>

                    <div className="card-header ">
                        <h5 className="card-title text-truncate">{this.props.carItem.name}</h5>
                    </div>
                    <ul className="list-group list-group-flush">

                        <li className="list-group-item card-text">

                            <Dropdown>
                                <Dropdown.Toggle variant="white" className="btn btn-light bg-white" id="dropdown-basic">
                                    <span>Milliage: </span>
                                    <span>{this.props.carItem.detailMileage.toString().replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ')}</span>
                                    <span> km</span>
                                </Dropdown.Toggle>

                                <Dropdown.Menu>
                                    <Dropdown.ItemText data-toggle="modal" href="#carEntityEditModalWindow" onClick={this.props.onEditButtonClick}>
                                        <h6 className="text-truncate ">
                                            <span>Replace after: </span>
                                            <b>{this.props.carItem.changeRide.toString().replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ')}</b>
                                            <b> km</b>
                                        </h6>
                                    </Dropdown.ItemText>
                                    <Dropdown.ItemText data-toggle="modal" href="#carEntityEditModalWindow" onClick={this.props.onEditButtonClick}>
                                        <h6 className="text-truncate">
                                            <span>Left: </span>
                                            <b>{(this.props.carItem.recomendedReplace - this.props.carItem.detailMileage).toString().replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ')}</b>
                                            <b> km</b>
                                        </h6>
                                    </Dropdown.ItemText>

                                </Dropdown.Menu>
                            </Dropdown>
                        </li>

                        <li className="list-group-item card-text">Price: {this.props.carItem.priceOfDetail.toString().replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ')} UAH</li>

                        <li className="list-group-item card-text">

                            <Dropdown>
                                <Dropdown.Toggle variant="white" className="btn btn-light bg-white" id="dropdown-basic">
                                    <span>
                                        Date: {this.state.replacedDate.toISOString().slice(0, 10)}
                                    </span>
                                </Dropdown.Toggle>

                                <Dropdown.Menu>
                                    <Dropdown.ItemText data-toggle="modal" href="#carEntityEditModalWindow" onClick={this.props.onEditButtonClick}>
                                        <span className="text-truncate">
                                            <span>Next replacement: </span>
                                            <b>{this.state.replaceAtDate.toISOString().slice(0, 10)}</b>
                                        </span>
                                    </Dropdown.ItemText>
                                    <Dropdown.ItemText data-toggle="modal" href="#carEntityEditModalWindow" onClick={this.props.onEditButtonClick}>
                                        <h6 className="text-truncate">
                                            <span>Left: </span>
                                            <b>{this.state.replaceAtDate.from(this.state.replacedDate)} </b>
                                        </h6>
                                    </Dropdown.ItemText>

                                </Dropdown.Menu>
                            </Dropdown>
                        </li>
                    </ul>
                    <div className="card-footer" >
                        <a className="card-link " data-toggle="modal" href="#carItemEditModalWindow" onClick={this.props.onEditButtonClick}>Edit</a>
                        <a className="card-link text-danger" data-toggle="modal" href="#carItemDeleteModalWindow" onClick={this.props.onEditButtonClick}>Delete</a>
                    </div>

                </div>

            </div>
        );
    }

}

export default ListItem;