import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})
export class AdminComponent {
  rooms = [
    { name: 'Room A', status: 'Available', capacity: 10, equipment: ['Mic', 'Projector'] },
    { name: 'Room B', status: 'Booked', capacity: 20, equipment: ['Projector'] },
    { name: 'Room C', status: 'Maintenance', capacity: 15, equipment: [] }
  ];

  newRoom = {
    name: '',
    capacity: 0,
    equipment: ''
  };

  addRoom() {
    this.rooms.push({
      name: this.newRoom.name,
      status: 'Available',
      capacity: this.newRoom.capacity,
      equipment: this.newRoom.equipment.split(',').map(e => e.trim())
    });
    this.newRoom = { name: '', capacity: 0, equipment: '' };
  }
}
