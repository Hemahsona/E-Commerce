import http from 'k6/http';
import { check } from 'k6';

export const options = {
    vus: 10,
    duration: '30s',
};

export default function () {
    const res = http.get('https://localhost:7180/api/Products');

    check(res, {
        'status is 200': (r) => r.status === 200,
    });
}
