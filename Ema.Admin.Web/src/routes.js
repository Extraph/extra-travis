import React from 'react';
import { Navigate } from 'react-router-dom';
import DashboardLayout from 'src/layouts/DashboardLayout';
import MainLayout from 'src/layouts/MainLayout';
import AccountView from 'src/views/account/AccountView';
import GenQrCodeListView from 'src/views/qrcode/GenQrCodeListView';

import LoginView from 'src/views/auth/LoginView';
import NotFoundView from 'src/views/errors/NotFoundView';
import RegisterView from 'src/views/auth/RegisterView';
import SettingsView from 'src/views/settings/SettingsView';
import ImportDataView from 'src/views/import/ImportDataView';
import TodayClassView from 'src/views/mobile/TodayClassView';
import NextClassView from 'src/views/mobile/NextClassView';
import NextClassDashView from 'src/views/mobile/NextClassDashView';
import ParticipantView from 'src/views/mobile/ParticipantView';
import ParticipantAdd from 'src/views/mobile/ParticipantAdd';
import ParticipantEdit from 'src/views/mobile/ParticipantEdit';
import CreditHourView from 'src/views/mobile/CreditHourView';
import CompanyView from 'src/views/master/CompanyView';
import CourseTypeView from 'src/views/master/CourseTypeView';
import RoleAssignmentView from 'src/views/master/RoleAssignmentView';
import CompanyAssignmentView from 'src/views/master/CompanyAssignmentView';

import GenReportListView from 'src/views/reports/GenReportListView';

const routes = [
  {
    path: 'app',
    element: <DashboardLayout />,
    children: [
      { path: 'account', element: <AccountView /> },
      { path: 'import', element: <ImportDataView /> },
      { path: 'genqrcode', element: <GenQrCodeListView /> },
      { path: 'today', element: <TodayClassView /> },
      { path: 'next', element: <NextClassView /> },
      { path: 'dash', element: <NextClassDashView /> },
      { path: 'participant', element: <ParticipantView /> },
      { path: 'participant/add', element: <ParticipantAdd /> },
      { path: 'participant/edit', element: <ParticipantEdit /> },
      { path: 'master/company', element: <CompanyView /> },
      { path: 'master/coursetype', element: <CourseTypeView /> },
      { path: 'master/roleassign', element: <RoleAssignmentView /> },
      { path: 'master/comassign', element: <CompanyAssignmentView /> },
      { path: 'credithour', element: <CreditHourView /> },
      { path: 'report', element: <GenReportListView /> },
      { path: 'settings', element: <SettingsView /> },
      { path: '*', element: <Navigate to="/404" /> }
    ]
  },
  {
    path: '/',
    element: <MainLayout />,
    children: [
      { path: 'login', element: <LoginView /> },
      { path: 'register', element: <RegisterView /> },
      { path: '404', element: <NotFoundView /> },
      { path: '/', element: <Navigate to="/login" /> },
      { path: '*', element: <Navigate to="/404" /> }
    ]
  }
];

export default routes;
