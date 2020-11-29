import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-proposal',
  templateUrl: './proposal.component.html',
  styleUrls: ['./proposal.component.scss']
})
export class ProposalComponent implements OnInit {
  @Input() text: string;
  @Input() votes: string;
  @Input() type: string;

  constructor() { }

  ngOnInit(): void {
  }

}
