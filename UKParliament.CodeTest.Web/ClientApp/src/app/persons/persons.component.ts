import {Component, Inject} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import { DatePipe } from '@angular/common';
import {PersonViewModel} from "../../models/person-view-model";
import { FormControl, AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import {catchError, Observable, of} from "rxjs";

@Component({
  selector: 'app-home',
  templateUrl: './persons.component.html',
  styleUrls: ['./persons.component.scss'],
  providers: [DatePipe]
})
export class PersonsComponent {
  people: PersonViewModel[] | undefined;
  person: PersonViewModel = { name: '', address: '', dateOfBirth: new Date(), id: 0  }

  personForm: FormGroup = new FormGroup({
    'id': new FormControl(),
    'name': new FormControl(),
    'dateOfBirth': new FormControl(),
    'address': new FormControl()
  });

  ngOnInit(): void {
    this.personForm = new FormGroup({
      id: new FormControl(this.person.id, [
        Validators.required,
      ]),
      name: new FormControl(this.person.name, [
        Validators.required,
        Validators.minLength(4)
      ]),
      address: new FormControl(this.person.address, [
        Validators.required,
        Validators.minLength(4)
      ]),
      dateOfBirth: new FormControl(this.person.dateOfBirth, [
        Validators.required
      ]),
    });
  }

  constructor(private http: HttpClient, @Inject('BASE_URL', ) private baseUrl: string, private datePipe: DatePipe) {
    this.getPeople();
  }
  getPeople(): void {
    this.http.get<PersonViewModel[]>(this.baseUrl + `api/person`).subscribe(result => {
      this.people = result;
    }, error => console.error(error));
  }

  onSubmit() {
    console.log('submitting');
    if (this.person.id === 0){
      // put this in angular service
      // @ts-ignore
      this.person.dateOfBirth = this.datePipe.transform(this.person.dateOfBirth, 'yyyy-MM-dd');

      this.http.post<any>(this.baseUrl + 'api/person/', this.personForm.value)
        .pipe(catchError((error: any, caught: Observable<any>): Observable<any> => {
          console.error('There was an error!', error);
          return of();
        }))
        .subscribe(data => {
          console.log('Success');
          this.getPeople();
        })
    }
    else
    {
      this.http.put(this.baseUrl + 'api/person/' + this.personForm.value.id, this.personForm.value).subscribe(result =>
      {
        console.log("updated a user... refreshing");
        this.getPeople();
      }, error => console.error(error));
    }
    this.person = { name: '', address: '', dateOfBirth: new Date(), id: 0  };
    this.personForm.reset();
    console.warn(this.personForm.value);
  }

  get name() { return this.personForm.get('name'); }
  get id() { return this.personForm.get('id'); }

  get dateOfBirth() { return this.personForm.get('dateOfBirth'); }
  get address() { return this.personForm.get('address'); }

  editPerson(id: number):void {
    // @ts-ignore
    this.person = this.people.find(f => f.id == id) as PersonViewModel;
    this.personForm.setValue(this.person);
    console.log('editing person ' + this.person.id);
  }
}
