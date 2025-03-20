import { Routes } from '@angular/router';
import { HomePage } from './pages/home/home.page';
import { ItemsPage } from './pages/items/items.page';
import { MonstersPage } from './pages/monsters/monsters.page';
import { AdminPage } from './pages/admin/admin.page';

export const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'home'
    },
    {
        path: 'home',
        component: HomePage
    },
    {
        path: 'items',
        component: ItemsPage
    },
    {
        path: 'monsters',
        component: MonstersPage
    },
    {
        path: 'admin',
        component: AdminPage
    },
];
