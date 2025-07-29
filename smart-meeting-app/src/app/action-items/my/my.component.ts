import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActionItemService, ActionItem } from '../../core/action-item.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-my-action-items',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my.component.html',
  styleUrls: ['./my.component.scss']
})
export class MyActionItemsComponent implements OnInit {
  items: ActionItem[] = [];
  loading = false;
  error: string | null = null;

  constructor(private actionItemService: ActionItemService, private router: Router) {}

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {
    this.loading = true;
    this.actionItemService.getMine().subscribe({
      next: (data) => {
        this.items = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load action items.';
        this.loading = false;
      }
    });
  }
goBack(): void {
  this.router.navigate(['/dashboard']);
}

  markCompleted(item: ActionItem): void {
    const updatedItem = {
      minutesID: item.minutesId,
      assignedTo: 0,
      description: item.description,
      dueDate: item.dueDate,
      status: 'Completed'
    };

    this.actionItemService.update(item.id, updatedItem).subscribe({
      next: () => this.loadItems(),
      error: () => alert('Failed to update status')
    });
  }
}
