import React, { Component, PureComponent } from 'react'
import 'bootstrap/dist/css/bootstrap.css'

class CarListItemRow extends Component
{
    render(){
        return(
            <tr className=" row">
            <th className="col-1">1</th>
            <td className="col-3 text-truncate">asd First item dsad  asd asd as dasdasd asdasdas asd asasfs dgfhgfg dfsdadfggff gdss</td>
            <td className="col-3">10 000</td>
            <td className="col-2">
                <input className="form-check-input" type="checkbox" value="" id="defaultCheck2" disabled />
            </td>
            <td className="col-3">

                <div className="dropdown">
                    <a className="btn btn-outline-info dropdown-toggle" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        View
                     </a>

                    <div className="dropdown-menu" aria-labelledby="dropdownMenuLink">
                        <a className="dropdown-item" href="#">Delete</a>
                        <a className="dropdown-item" href="#">Edit</a>
                    </div>
                </div>

            </td>
        </tr>
        );
    }
}

export default CarListItemRow