
import React, { Component, PureComponent } from 'react'
import 'bootstrap/dist/css/bootstrap.css'
import Dropdown from 'react-bootstrap/Dropdown';


class ListItem extends Component {

    render() {
        return (
            <div className="p-2 col-md-2" >

                <div className={`card border-secondary text-center ${Number.parseInt(this.props.carItem.ride) > Number.parseInt(this.props.carItem.recommendedRide) ? "border-danger" : ""} `}>

                    <div className="card-header ">
                        <h5 className="card-title text-truncate">{this.props.carItem.title}</h5>
                    </div>
                    <ul className="list-group list-group-flush">
                       
                        <li className="list-group-item card-text">

                            <Dropdown>
                                <Dropdown.Toggle variant="white" className="btn btn-light bg-white" id="dropdown-basic">
                                    <span>Milliage: </span>
                                    <span>{String(this.props.carItem.ride).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ')}</span>
                                    <span> km</span>
                                </Dropdown.Toggle>

                                <Dropdown.Menu>
                                    <Dropdown.ItemText data-toggle="modal" href="#carEntityEditModalWindow" onClick={this.props.onEditButtonClick}>
                                        <h6 className="text-truncate ">
                                            <span>Replace after: </span>
                                            <b>{String(Number(this.props.carItem.ride) + 1000000).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ')}</b>
                                            <b> km</b>
                                        </h6>
                                    </Dropdown.ItemText>
                                    <Dropdown.ItemText data-toggle="modal" href="#carEntityEditModalWindow" onClick={this.props.onEditButtonClick}>
                                        <h6 className="text-truncate">
                                            <span>Left: </span>
                                            <b>{String(Number(this.props.carItem.ride) - 10000).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ')}</b>
                                            <b> km</b>
                                        </h6>
                                    </Dropdown.ItemText>

                                </Dropdown.Menu>
                            </Dropdown>
                        </li>

                        <li className="list-group-item card-text">Price: {String(this.props.carItem.price).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ')} UAH</li>

                        <li className="list-group-item card-text">

                            <Dropdown>
                                <Dropdown.Toggle variant="white" className="btn btn-light bg-white" id="dropdown-basic">
                                    <span>
                                        Date: {this.props.carItem.date}
                                    </span>
                                </Dropdown.Toggle>

                                <Dropdown.Menu>
                                    <Dropdown.ItemText data-toggle="modal" href="#carEntityEditModalWindow" onClick={this.props.onEditButtonClick}>
                                        <span className="text-truncate">
                                            <span>Next replacement: </span>
                                            <b>{this.props.carItem.date}</b>
                                        </span>
                                    </Dropdown.ItemText>
                                    <Dropdown.ItemText data-toggle="modal" href="#carEntityEditModalWindow" onClick={this.props.onEditButtonClick}>
                                        <h6 className="text-truncate">
                                            <span>Left: </span>
                                            <b>6 days 4 month 1 year </b>
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