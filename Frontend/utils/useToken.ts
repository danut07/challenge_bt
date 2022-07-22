import axios from "axios";
import { useState } from "react";
import { Token } from "./interface";
const https = require("https");

export const useToken = () => {
  const [token, setToken] = useState<Token>();

  const handleGenerateToken = async (
    username: string,
    unixtimestamp: number
  ) => {
    // SSL verification is disabled for DEVELOPMENT
    const httpsAgent = new https.Agent({ rejectUnauthorized: false });

    await axios
      .post(
        `https://localhost:7195/generateToken`,
        {
          Username: username,
          Timestamp: unixtimestamp,
        },
        {
          httpsAgent: httpsAgent,
          headers: {
            "Content-Type": "application/json",
            "Accept-Encoding": "gzip, deflate, br",
          },
        }
      )
      .then((res) => {
        console.log("this is res data", res.data);
        setToken({
          expiryTime: res.data.expiryTime,
          totp: res.data.totp,
        });
      });
  };

  return { handleGenerateToken, token };
};
