import { Text, Input, Flex, Button, CircularProgress } from "@chakra-ui/react";
import React, { useEffect, useState } from "react";
import { Content, FormView } from "../components/useComponents";
import { PassText } from "../components/useTypography";
import { useToken } from "../utils/useToken";

export const HomeContent = () => {
  const tokenHooks = useToken();

  const [username, setUsername] = useState<string>("");
  const [time, setTime] = useState<any>(-1);

  const handleUsernameChange = (e: any) => {
    setUsername(e.target.value);
  };

  useEffect(() => {
    let remaining_time =
      (tokenHooks.token?.expiryTime || 0) - Math.floor(Date.now() / 1000) > 0
        ? (tokenHooks.token?.expiryTime || 0) - Math.floor(Date.now() / 1000)
        : 0;
    console.log({
      remaining_time,
      real: (tokenHooks.token?.expiryTime || 0) - Math.floor(Date.now() / 1000),
      current_time: Math.floor(Date.now() / 1000),
    });

    if (tokenHooks.token === void 0) {
      remaining_time = -1;
    }

    setTime(remaining_time);

    if (remaining_time >= 0) {
      setTimeout(() => {
        setTime(remaining_time - 1);
      }, 1000);
    }
  }, [Math.floor(Date.now() / 1000)]);

  return (
    <Content>
      <Flex>#BTChallenge</Flex>
      <FormView>
        <Text my={2}>Username</Text>
        <Input
          name="username"
          value={username}
          onChange={handleUsernameChange}
        />
        <Button
          mt="40px"
          w="10vw"
          alignSelf="center"
          onClick={() => {
            tokenHooks.handleGenerateToken(
              username,
              Math.floor(Date.now() / 1000)
            );
          }}
        >
          Generate
        </Button>

        <Flex direction="inherit" justify="center" align="center" mt="64px">
          <PassText>{tokenHooks.token?.totp}</PassText>
        </Flex>

        <Flex direction="inherit" justify="center" align="center" mt="64px">
          <PassText>{time === -1 ? `` : time > 0 ? time : `expired`}</PassText>
        </Flex>
      </FormView>
    </Content>
  );
};
