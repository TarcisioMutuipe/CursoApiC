import { NgModule, Component } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AlunosComponent } from './components/alunos/alunos.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { GraficosComponent } from './components/graficos/graficos.component';
import { CorretorasComponent } from './components/corretoras/corretoras.component';
import { PerfilComponent } from './components/perfil/perfil.component';
import { ProfessoresComponent } from './components/professores/professores.component';
import { ProfessorDetalheComponent } from './components/professores/professor-detalhe/professor-detalhe.component';



const routes: Routes = [
  { path: 'alunos', component: AlunosComponent },
  { path: 'alunos/:id', component: AlunosComponent },
  { path: 'perfil', component: PerfilComponent },
  { path: 'professores', component: ProfessoresComponent },
  { path: 'professor/:id', component: ProfessorDetalheComponent },
  {path:'dashboard',component: DashboardComponent},
  {path:'corretoras',component: CorretorasComponent},
  {path:'graficos',component: GraficosComponent},
  {path:'',redirectTo:'dashboard',pathMatch: 'full'},
  {path:'**',redirectTo:'dashboard',pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
