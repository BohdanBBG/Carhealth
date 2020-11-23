import React, { Component, PureComponent } from 'react'
import 'bootstrap'

class CarListItemRow extends Component
{
    render(){
        return(
            <tr className=" row">
            <th className="col-1">{this.props.index}</th>
            <td className="col-3 text-truncate">{this.props.listItem.title}</td>
            <td className="col-3">{String(this.props.listItem.ride).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ') }</td>
            <td className="col-2">
                <input className="form-check-input" type="checkbox" value="" id="defaultCheck2" checked={this.props.listItem.isCurrent} readOnly/>
            </td>
            <td className="col-3">

                <div className="dropdown">
                    <a className="btn btn-outline-info dropdown-toggle" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        Additionally
                     </a>

                    <div className="dropdown-menu" aria-labelledby="dropdownMenuLink">
                        <a className="dropdown-item" data-toggle="modal" href="#carEntityDeleteModalWindow" onClick={this.props.onEditButtonClick}>Delete</a> 
                        <a className="dropdown-item" data-toggle="modal" href="#carEntityEditModalWindow" onClick={this.props.onEditButtonClick}>Edit</a>
                    </div>
                </div>

            </td>
        </tr>
        );
    }
}

export default CarListItemRow