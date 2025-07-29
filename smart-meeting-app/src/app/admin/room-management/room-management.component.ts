import { Component, OnInit } from '@angular/core';
import { Room, RoomService, CreateRoomDto } from '../../core/room.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-room-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './room-management.component.html',
  styleUrls: ['./room-management.component.scss']
})
export class RoomManagementComponent implements OnInit {
  rooms: Room[] = [];
  newRoom: CreateRoomDto = { name: '', capacity: 0, location: '', features: '' };
  editingRoomId: number | null = null;
  error = '';
  success = '';

  constructor(private roomService: RoomService, private router: Router) {}
 goToDashboard(): void {
    this.router.navigate(['/admin']);
  }
  ngOnInit(): void {
    this.loadRooms();
  }

  loadRooms(): void {
    this.roomService.getAllRooms().subscribe({
      next: (rooms) => this.rooms = rooms,
      error: () => this.error = 'Failed to load rooms.'
    });
  }

  saveRoom(): void {
    this.error = '';
    this.success = '';

    if (this.editingRoomId === null) {

      this.roomService.createRoom(this.newRoom).subscribe({
        next: () => {
          this.success = 'Room created successfully!';
          this.newRoom = { name: '', capacity: 0, location: '', features: '' };
          this.loadRooms();
        },
        error: () => this.error = 'Failed to create room.'
      });
    } else {

      this.roomService.updateRoom(this.editingRoomId, this.newRoom).subscribe({
        next: () => {
          this.success = 'Room updated successfully!';
          this.editingRoomId = null;
          this.newRoom = { name: '', capacity: 0, location: '', features: '' };
          this.loadRooms();
        },
        error: () => this.error = 'Failed to update room.'
      });
    }
  }

  editRoom(room: Room): void {
    this.editingRoomId = room.id;
    this.newRoom = { ...room };
  }

  deleteRoom(id: number): void {
    if (confirm('Are you sure you want to delete this room?')) {
      this.roomService.deleteRoom(id).subscribe({
        next: () => this.loadRooms(),
        error: () => this.error = 'Failed to delete room.'
      });
    }
  }

  cancelEdit(): void {
    this.editingRoomId = null;
    this.newRoom = { name: '', capacity: 0, location: '', features: '' };
  }
}
