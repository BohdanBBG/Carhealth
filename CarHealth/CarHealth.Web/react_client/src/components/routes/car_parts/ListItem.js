
import React, { Component, PureComponent } from 'react'
import 'bootstrap/dist/css/bootstrap.css'


class ListItem extends Component {

    render() {
        return (
            <div className="p-2 col-md-2" >

                <div className={`card text-center ${ Number.parseInt(this.props.carItem.ride) > Number.parseInt(this.props.carItem.recommendedRide) ? "border-danger" : "" } `}>

                    <div className="card-header ">
                        <h5 className="card-title text-truncate">{this.props.carItem.title}</h5>
                    </div>
                    <ul className="list-group list-group-flush">
                        <li className="list-group-item card-text">
                            <span>Ride: </span>
                            <span>{String(this.props.carItem.ride).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ') }</span>
                            <span> km</span>
                        </li>
                        <li className="list-group-item card-text">Price: {String(this.props.carItem.price).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ')} UAH</li>
                        <li className="list-group-item card-text">Date: {this.props.carItem.date}</li>
                    </ul>
                    <div className="card-footer">
                        <a href="#" className="card-link ">Edit</a>
                        <a href="#" onClick={this.handleDeleteButton.bind(this)} className="card-link text-danger">Delete</a>
                    </div>

                </div>


            </div>
        );
    }

    handleDeleteButton = (e) => alert("Bi");
}

export default ListItem;