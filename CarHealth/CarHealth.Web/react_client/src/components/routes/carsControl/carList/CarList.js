
import React, { Component, PureComponent } from 'react'
import 'bootstrap/dist/css/bootstrap.css'
import CarListItemRow from './CarListItemRow.js'

class CarList extends Component {


    render() {

        

        return (
            // <div className="col-3 text-center pt-5 pb-5 ">
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
                       <CarListItemRow></CarListItemRow>
                       <CarListItemRow></CarListItemRow>
                       <CarListItemRow></CarListItemRow>
                       <CarListItemRow></CarListItemRow>

                    </tbody>
                </table>

            </div>

        )
    };
}

export default CarList;