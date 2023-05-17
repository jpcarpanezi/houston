import { Component } from '@angular/core';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent {
	public isSearchBarOpen: boolean = false;

	toggleSearchBar(): void {
		this.isSearchBarOpen = !this.isSearchBarOpen;
	}
}
