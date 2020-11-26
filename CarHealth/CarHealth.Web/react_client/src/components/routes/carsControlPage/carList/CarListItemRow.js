
import React, { Component, PureComponent } from 'react'


class CarListItemRow extends Component {


    render() {
        return (
            <tr >
                <th >{this.props.index}</th>
                <td className="text-truncate">{this.props.listItem.title}</td>
                <td >{String(this.props.listItem.ride).replace(/(\d)(?=(\d{3})+([^\d]|$))/g, '$1 ')}</td>
                <td >
                    <input className="form-check-input" type="checkbox" value="" id="defaultCheck2" checked={this.props.listItem.isCurrent} readOnly />
                </td>
                <td>
                    <div class="dropdown">
                        <button class="btn btn-outline-secondar dropdown-toggle"
                            type="button" id="dropdownMenuButton" data-toggle="dropdown"
                            aria-haspopup="true" aria-expanded="false">
                            <i class="fas fa-cog"></i>
                        </button>
                        <div class="dropdown-menu" aria-labelledby="dropdownMenuLink">
                            <a class=" dropdown-item" data-toggle="modal" href="#carEntityEditModalWindow" onClick={this.props.onEditButtonClick}>
                                <i class="fas fa-pen"></i>
                                <span>Edit </span>
                            </a>

                            <a class="dropdown-item" data-toggle="modal" href="#carEntityDeleteModalWindow" onClick={this.props.onEditButtonClick}>
                                <i class="fas fa-trash"></i>
                                <span>Delete </span>
                            </a>
                        </div>
                    </div>
                </td>
            </tr>
        );
    }
}

export default CarListItemRow