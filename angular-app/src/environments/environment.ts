export const environment = {
    production: false,
    presentationUrl: 'http://localhost:5001/api/presentation',
    auth: {
        domain: "dev-y2t083gd3qk2m86l.us.auth0.com",
        clientId: "ttwWjKP6F9mllvP9sp9ztp764iGwqbv3",
        authorizationParams: {
            audience: "https://souls-like-presentation-api",
            redirect_uri: window.location.origin
        },
        apiUri: "http://localhost:5001",
        appUri: "http://localhost:4200",
        errorPath: "/error",
    },
    httpInterceptor: {
        allowedList: [`http://localhost:5001/api/presentation/*`],
    },
}