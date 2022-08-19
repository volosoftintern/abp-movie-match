import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'MovieMatch',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44389',
    redirectUri: baseUrl,
    clientId: 'MovieMatch_App',
    responseType: 'code',
    scope: 'offline_access MovieMatch',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44389',
      rootNamespace: 'MovieMatch',
    },
  },
} as Environment;
