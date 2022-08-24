import { Environment } from '@abp/ng.core';

const baseUrl = 'https://volosoftintern.github.io/abp-movie-match';//github pages link
export const imagePath='https://image.tmdb.org/t/p/original/'
export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'MovieMatch',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://abpmoviematch.azurewebsites.net',
    redirectUri: baseUrl,
    clientId: 'MovieMatch_App',
    responseType: 'code',
    scope: 'offline_access MovieMatch',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://abpmoviematch.azurewebsites.net',
      rootNamespace: 'MovieMatch',
    },
  },
} as Environment;
